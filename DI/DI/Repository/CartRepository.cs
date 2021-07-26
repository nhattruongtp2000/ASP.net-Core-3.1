using BraintreeHttp;
using Data.Data;
using Data.DB;
using Data.Utilities.Constants;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PayPal.Core;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;


namespace DI.DI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly Iden2Context _iden2Context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly string _clientId;
        private readonly string _secretKey;
        public CartRepository(Iden2Context iden2Context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IConfiguration config)
        {
            _iden2Context = iden2Context;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _clientId = "AV-BEqQ4nyfYnUsK6_tkEim9gvX_wWaldPQETeF8DqUg6StRBlikt07ap6efAfcUd477BY77DmZ-ZNMN";
            _secretKey = /*config["PaypalSettings:SecretKey"];         */  "ENt8I6wQejmZJpmoe10v1Ah-q8G16mBsCjpVcgQsvIHbhLmGDXiC9K-hDJFFqQbVhi9m427R3QUNqI27";
        }

        public double TyGiaUSD = 23300;//store in Database
        public void AddtoCart(int IdProduct)
        {
            var x = _iden2Context.Products.Where(x => x.IdProduct == IdProduct).FirstOrDefault();

            ProductVm product2 = new ProductVm() 
            { 
            DateAccept=x.DateAccept,
            IdBrand=x.IdBrand,
            IdCategory=x.IdCategory,
            IdProduct=x.IdProduct,
            PhotoReview=x.PhotoReview,
            Price=x.Price,
            ProductName=x.ProductName,
            UseVoucher=x.UseVoucher,
            };
            var cart = GetCartItems();
            var cartItems = cart.Find(x => x.Product.IdProduct == IdProduct);
            if (cartItems != null)
            {
                cartItems.Quantity++;
            }
            else
            {
                cart.Add(new CartItem() { Quantity = 1, Product = product2 });
            }
            SaveCart(cart);


        }

        public void ClearCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove(SystemConstants.AppSettings.CARTKEY);
        }


        public void RemoveCart(int IdProduct)
        {
            var cart = GetCartItems();
            var cartItem = cart.Find(x => x.Product.IdProduct == IdProduct);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }
            SaveCart(cart);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        public void SaveCart(List<CartItem> ls)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(SystemConstants.AppSettings.CARTKEY,jsoncart);
        }

        public void UpdateCart(int IdProduct, int Quantity)
        {
            var carts = GetCartItems();
            var cartItems = carts.Find(x => x.Product.IdProduct == IdProduct);
            if (cartItems != null)
            {
                cartItems.Quantity = Quantity;
            }
            SaveCart(carts);
        }

        public  List<CartItem> GetCartItems()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string jsoncart = session.GetString(SystemConstants.AppSettings.CARTKEY);
            if (jsoncart != null)
            {
                var a = JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
                return a;
            }
            return new List<CartItem>();
        }

        public async Task<List<ProductVm>> GetAll()
        {
            var query = await _iden2Context.Products.Select(x => new ProductVm()
            {
                DateAccept = x.DateAccept,
                IdBrand = x.IdBrand,
                IdCategory = x.IdCategory,
                IdProduct = x.IdProduct,
                PhotoReview = x.PhotoReview,
                Price = x.Price,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
            }).ToListAsync();
            return query;
        }

      

        public async Task<string> Purchase(int total)
        {
            Random generator = new Random();
            string IdOrder = generator.Next(0, 1000000).ToString("D6");
            

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string Id = user.Id;
            //var freeProduct = await _iden2Context.Products.Where(x => x.IsFree==true).FirstOrDefaultAsync();

            var b = GetCartItems();
            foreach (var item in b)
            {
                var orderdetails = new OrderDetails()
                {
                    IdOrder = IdOrder,
                    IdProduct = item.Product.IdProduct,
                    StatusDetails = Data.Enums.Status.Process,
                    Price =item.Product.Price*item.Quantity,                   
                    Quality = item.Quantity
                };
                _iden2Context.OrderDetails.Add(orderdetails);
            }
            var a = new Data.Data.Order()
            {
                IdOrder= IdOrder,
                IdUser = Id,
                Status = Data.Enums.Status.Process,
                OrderDay = DateTime.Now,
                TotalPice = total,
                PaymentType= "Direct Payment"
            };
            if (a.TotalPice >= 1000)
            {
                a.TotalPice = a.TotalPice * 90 / 100;
            }
            //if (a.TotalPice >= 10000)
            //{
            //    var orderdetails = new OrderDetails()
            //    {
            //        IdOrder = IdOrder,
            //        IdProduct = freeProduct.IdProduct,
            //        StatusDetails = Data.Enums.Status.Process,
            //        Price = 0,
            //        Quality = 0
            //    };
            //    _iden2Context.OrderDetails.Add(orderdetails);
            //}
            _iden2Context.Orders.Add(a);
            ClearCart();
            await _iden2Context.SaveChangesAsync();
            return IdOrder;
        }

        public async Task<List<OrderDetailsVm>> Checkout(string IdUser)
        {
            var a = from p in _iden2Context.Orders
                    join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                    join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct
                    select new { p, pt,ptt };
            var order = a.Where(x => x.p.IdUser == IdUser);
            var checkout = await order.Select(x => new OrderDetailsVm()
            {
                IdOrder = x.p.IdOrder,
                StatusDetails = x.pt.StatusDetails,
                IdProduct = x.pt.IdProduct,
                Price = x.pt.Price,
                Quality = x.pt.Quality,
                DateOrder = x.p.OrderDay,
                PhotoReview = x.ptt.PhotoReview
            }).ToListAsync();
            return checkout;
        }

        public async Task<string> PayPal(double total)
            {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);
            var Carts = GetCartItems();
            total = Math.Round(total/TyGiaUSD, 2);

            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };

            foreach(var item in Carts)
            {
                itemList.Items.Add(new Item() 
                {
                Name=item.Product.ProductName,
                Currency="USD",
                Price=Math.Round(item.Product.Price/TyGiaUSD,2).ToString(),
                Quantity=item.Quantity.ToString(),
                Sku="sku",
                Tax="0"               
                });
            }

            //payment

            Random generator = new Random();
            string paypalOrderId = generator.Next(0, 1000000).ToString("D6");
            var payment = new Payment()
            {
                Intent = "sale",

                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount=new Amount()
                        {
                            Total=total.ToString(),
                            Currency="USD",
                            Details=new AmountDetails
                            {
                                Tax="0",
                                Shipping="0",
                                Subtotal=total.ToString()
                            }
                        },
                        ItemList=itemList,
                        Description="descrip",
                        InvoiceNumber=paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = "https://localhost:5002/Cart/CheckoutFail",
                    ReturnUrl = "https://localhost:5002/Cart/CheckoutSuccess"
                }               ,
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
              var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }
                return paypalRedirectUrl;
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return "/Cart/CheckoutFail";

            }
        }

        public async Task<string> CheckoutSuccess(decimal total)
        {
            Random generator = new Random();
            string IdOrder = generator.Next(0, 1000000).ToString("D6");


            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string Id = user.Id;
            var freeProduct = await _iden2Context.Products.Where(x => x.IsFree == true).FirstOrDefaultAsync();

            var b = GetCartItems();
            foreach (var item in b)
            {
                var orderdetails = new OrderDetails()
                {
                    IdOrder = IdOrder,
                    IdProduct = item.Product.IdProduct,
                    StatusDetails = Data.Enums.Status.Process,
                    Price = item.Product.Price * item.Quantity,
                    Quality = item.Quantity
                };
                _iden2Context.OrderDetails.Add(orderdetails);
            }
            var a = new Data.Data.Order()
            {
                IdOrder = IdOrder,
                IdUser = Id,
                Status = Data.Enums.Status.Process,
                OrderDay = DateTime.Now,
                TotalPice = total,
                PaymentType = "PayPal Payment"
            };

            _iden2Context.Orders.Add(a);
            ClearCart();
            await _iden2Context.SaveChangesAsync();
            return IdOrder;
        }

        public Task<string> CheckoutFail()
        {
            throw new NotImplementedException();
        }
    }
}

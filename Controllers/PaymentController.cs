using BusBooking.Context;
using BusBooking.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace BusBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly BusBookingDbContext _context;
 

        public PaymentController(IConfiguration config, BusBookingDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("createOrder")]

        public IActionResult CreateOrder([FromBody] PaymentRequest obj)
        {
            var client = new RazorpayClient(
                _config["Razorpay:key"],
                _config["Razorpay:secretKey"]
                );

            Dictionary<string, object> options = new Dictionary<string, object>
            {
                {"amount", obj.Amount*100 },
                {"currency", "INR"},
                {"receipt", "order_rcptid_11"}
            };

            Razorpay.Api.Order order= client.Order.Create(options);
            return Ok(new
            {
                orderId = order["id"].ToString(),
                amount = order["amount"],
                currency = order["currency"]
            });
        }

        [HttpPost("verify-payment")]
        public IActionResult VerifyPayment([FromBody] RazorpayPaymentResponse obj)
        {
            var client = new RazorpayClient(
                _config["Razorpay:key"],
                _config["Razorpay:secretKey"]
                );

            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("razorpay_order_id", obj.razorpay_order_id);
            options.Add("razorpay_payment_id", obj.razorpay_payment_id);
            options.Add("razorpay_signature", obj.razorpay_signature);

            Utils.verifyPaymentSignature(options);
            return Ok(new { message = "Payment verified successfully" });
        }

        [HttpPost("save-payment")]
        public async Task<IActionResult> SavePayment([FromBody] PaymentDTO dto)
        {
            if (dto == null || dto.BookingId == 0 || string.IsNullOrWhiteSpace(dto.RazorpayPaymentId))
            {
                return BadRequest("Invalid payment data.");
            }

            var payment = new BusBooking.Models.Payment
            {
                BookingId = dto.BookingId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = dto.PaymentStatus,
                PaymentDate = dto.PaymentDate,

                RazorpayPaymentId = dto.RazorpayPaymentId,
                RazorpayOrderId = dto.RazorpayOrderId,
                RazorpaySignature = dto.RazorpaySignature
            };

            _context.payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok(new { paymentId = payment.PaymentId });
        }


        public class PaymentRequest 
        {
            public int Amount { get; set; }

        }

        public class RazorpayPaymentResponse
        {
            public string razorpay_order_id { get; set; }
            public string razorpay_payment_id { get; set; }
            public string razorpay_signature { get; set; }  
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class ZFBResDto
    {
        private string app_id = "2018041302550006";   //开发者的应用ID
        private string format = "JSON";
        private string charset = "utf-8";
        private string sign_type = "RSA2";  //签名格式
        private string version = "1.0";
        //商户私钥
        private string merchant_private_key = "MIIEpQIBAAKCAQEA0wSiER8RQygVK1/1j5nYAzDJwI6Tx9NL3j7YfecRZvfXCJ8oTysByONG/loL0vdl/Ih0oeIwDcNbUr1F4AgjnQYRz8l16UVu8AwYShqf29MGfSAQLdvzqG3/oqkJpNwaBMALtyq1qwklga5fEgH8/l+pFc+LN2aTaxtWFShaDQRpMuVGsE3CwzALK+XWZxm49XcbBikH04F65Kx0NmC4FziBT/HS/kCC5atJ2JiUoa2VM8j00CBM0On/vHodjU3HVmmBg1LBVoTi9+FbaJT+Hdqy4H0In1V+2K9OVREe8ZrURT6S0zAeTgrjAUTKt9PMYrpkCaDkVqHzIhOB2gruvwIDAQABAoIBAQDBSVciE7D+MLLjXixR8vs4QPIsXOzkdpjh4/LtsD/yb0YacZ68lYo29mfLB7QY8+AJJvyeY87cbHs0GIbupMXqSOr7x28n0x/A5XNCPYz8EBm7dykauIRBXTBxUCCzT6DNhRO2HXr2RZSDarNOjV+tqPX6Mnc0sdKKoymAi8ugayUaqn8dTajoqg+drk6L1IgZCMK/CmVYPYJIQ+VrvRgwQ7bYiC1BHCme7I/n43rxGOQaXB9wRK5/LSD9O0tR9QktvhbsmtwW+e5TvoAGpy9p0LQMhLSdj5CocnM0G11dIboECrYc+SYD841EJF4J2pm18VIPp3rvoYZxRXrFleSxAoGBAPkGkmnawaHk/IhHyX0ZuHVMwBf8l4ZK3ducpBLKRdtLQvW2XOuoQhNiuKXg7KOW9npvN/GwKC50V8l7RblbEoZngyJ8rjm00sRF0UtSbWOdyII5WF0/mwJGm8yiwq8GuSyHj0xAdeJopH53ybuejBfN8VeBzv4H1G9dUuLnbCxVAoGBANjtj1oKsE7LrjNkiIN1+3P+wKIy/TwqA5bLwUILnzVW92IC2gHQLN+NoksKngkgH7m8Xnp0Eue3anUNfeAgdJKar94bKiBNest4ZOk288w0E5kRr2X/UtlVLIYcuJ4o9bx+QDn+ftMlWk90WOQI14QQxswZCa1fHe9b/60VmoLDAoGBAJqxSWx2VsiB3Zmutmx++MXtGnsMDvh+M1lD8ew2OLTkCMFoOkqtp/Yw4jExCu8ITS57Pk5ltmA9J3dim0psV5KkZKKcvwHb4P3JvRzEJG24SyESDGFIrLr6L7gr9zIQxCD0SMD+Xfx6MozZTri84Zu788r/OR02sfFIEMAhMGJNAoGAc4GK4xbt6gbqKtNNHTKlQY5UZAlibbaxUooLzW8CxxQXhUifbHe8bQytbeepXpKMUgnLBMjpiBhRxyH39G9TovxayJkORUT8LXtdwBBSoFjaVpbkHhtlsfN4UbDZXN3Sext+d2LbhPJOtB/vdPyARQHp2KM8U+RhvCHwceke7KECgYEAsdikO13ps6ML6buBTOZ3/3lqJVmNkUo9YG6gpxzhq/RQ2dXPqRqvJtDr/qmjP1+kigOi9xr8ZifjBEUI1ymTv/Th3X3Rq+o6Hrdfr1TpqE8ADlnJQYc6noV0qdwkd8lsnToLCrpUNLmU0EKWaIbiRv3+E55H80A3oyKGufqIlBo=";
        //支付宝公钥
        private string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAydy5QG+mxrN77GIiLLEis/cVYleYn8hg5CesKKT73WkswNSr4ohTfV/BVvHdURec395Ex2T230mKu/QSbbY7Ahi3k+EWufIX6TYz/Q28grV4BrtF1kIcNvQUHgE/zEGL+nVQhfEH/uBjZQ84ue4S8ywnVBovndB2rJOExr1SBS5iI1yzYxdvpHRGTHHMIF0MInW96dRU1t3XqGut6e4YvsZj8x3tEprNBSF5MIQ+BRz9KrdplIRpzR/sPXenxbgGjBTj5Fus/a625Oofb23F6W8qPG86m9RgPo14BWmAi5SoVa7FAYubN2p3OBpnEhxkDqSq5LmtVHkXUKOjxaIQxwIDAQAB";

        /// <summary>
        /// 开发者的应用ID
        /// </summary>
        public string AppId { get { return app_id; } }

        public string Format { get { return format; } }

        public string CharSet { get { return charset; } }

        public string SignType { get { return sign_type; } }

        public string Version { get { return version; } }

        public string AlipayPublicKey { get { return alipay_public_key; } }

        public string MerchantPrivateKey { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }
    }
}

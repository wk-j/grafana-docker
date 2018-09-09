using Microsoft.AspNetCore.Mvc;

namespace MyWeb {
    public class Data {
        public int I1 { set; get; }
        public int I2 { set; get; }
    }

    [Route("api/[controller]/[action]")]
    public class HelloController {

        [HttpGet]
        public Data A1() {
            return new Data();
        }

        [HttpGet]
        public Data A2() {
            return new Data();
        }
    }
}
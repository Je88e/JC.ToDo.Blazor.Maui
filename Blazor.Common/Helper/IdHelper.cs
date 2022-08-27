namespace Blazor.Common.Helper
{
    /*
    唯一ID位数为64bit 每个bit的含义如下

         1bit| 5bit  | 40bit | 6bit  | 12bit |
         | - | ----- | ----- | ----- | ----- |
         |63 |62..58 |57...18|17...12|11...0 |
         | 0 | 0...0 | 0...0 | 0...0 | 0...0 |
         | - | ----- | ----- | ----- | ----- |
         | ① |   ②   |  ③    |   ④   |  ⑤    |

   ①：符号位不用
   ②：唯一号类型，可以定义32种类型
   ③：时间戳，从2020-05-26（自定义的时间基准）到现在的毫秒数，可以表示34年左右
   ④：应用id，可以表示64个应用或者前几比特表示服务器，后几比特表示应用
   ⑤：序列号，每毫秒内序列号有序，每毫秒可以产生4095个序列号
   最大数量  4095000个/秒   4095个/毫秒
    */
    //通用ID生成助手
    public class IdHelper
    {
        private static long seq = 0;
        private static object GeneralNoLock = new object();
        //时间基准
        private static long basicTicks = new DateTime(2019, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
        private static long appId = 1; //在配置文件中配置appId
        private static long timePart = 0;

        //type的常量  0-31
        private const long TYPE_VipId = 1;                 //会员编号

        /// <summary>
        /// 通用生成唯一号方法
        /// </summary>
        /// <param name="type">唯一号类型 0-31</param>
        /// <param name="appId">应用程序编号 0-63</param>
        private static long GenaralNo(long type, long appId)
        {
            lock (GeneralNoLock)
            {
                long time = (DateTime.Now.Ticks - basicTicks) / 10000;
                if (time != timePart)
                {
                    timePart = time;
                    seq = 0;
                }
                seq++;
                if (seq > 4095) throw new Exception("唯一号超出最大生成数");
                long uid = (type << 58) +
                           (time << 18) +
                           (appId << 12) +
                           seq;
                return uid;
            }
        }

        public static long VipId()
        {
            return GenaralNo(TYPE_VipId, appId);
        }
    }

}

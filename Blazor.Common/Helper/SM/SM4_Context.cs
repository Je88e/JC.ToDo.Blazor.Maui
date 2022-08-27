namespace Blazor.Common.Helper.SM
{
    public class SM4_Context
    {
        public int mode;

        public long[] sk;

        public bool isPadding;

        public SM4_Context()
        {
            mode = 1;
            isPadding = true;
            sk = new long[32];
        }
    }
}

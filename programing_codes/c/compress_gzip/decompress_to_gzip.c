#include<zlib.h>

int decompress_to_gzip(const unsigned char *input, unsigned long input_len, unsigned char **output, unsigned long *output_len)
{
    z_stream strm;
    int ret;
    
    //예상 Size = 압축이 25배 되어 있다고 가정함
    unsigned long guess_size = input_len * 25;
    if (guess_size < 1024)
    { guess_size = 1024; }
    *output = (unsigned char *)malloc(guess_size);
    if (output == NULL)
    { return -1; }
    
    strm.zalloc = Z_NULL;
    strm.zfree = Z_NULL;
    strm.opaque = Z_NULL;
    strm.avail_in = input_len;
    strm.next_in = (unsigned char *)input;
    strm.avail_out = guess_size;
    strm.next_out = *output;
    
    ret = inflateInit2(&strm, 31);
    if (ret != Z_OK)
    {
        free(*output);
        return -1;
    }
    ret = inflate(&strm, Z_FINISH);
    if (ret != Z_STREAM_END)
    {
        // 압축 해제 실패 Buffer Size가 너무 작음
        inflateEnd(&strm);
        free(*output);
        return -2;
    }
    *output_len = strm.total_out;
    inflateEnd(&strm);
    
    return 0;
}
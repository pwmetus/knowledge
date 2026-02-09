#include<zlib.h>

int compress_to_gzip(const char *input, unsigned long input_len, unsigned char** output, unsigned long *output_len)
{
    z_stream strm;
    strm.zalloc = Z_NULL;
    strm.zfree = Z_NULL;
    strm.opaque = Z_NULL;
    
    if(deflateInit2(&strm, Z_DEFAULT_COMPRESSION, Z_DEFLATED, 31, 8, Z_DEFAULT_STRATEGY) != Z_OK)
    { return -1; }
    
    unsigned long max_dest_len = compressBound(input_len) + 18;
    *output = (unsigned char *)malloc(max_dest_len);
    
    strm.next_in = (unsigned char *)input;
    strm.avail_in = input_len;
    strm.next_out = *output;
    strm.avail_out = max_dest_len;
    
    int ret = deflate(&strm, Z_FINISH);
    
    if (ret != Z_STREAM_END)
    {
        deflateEnd(&strm);
        free(*output);
        return -1;
    }
    *output_len = strm.total_out;
    deflateEnd(&strm);
    
    return 0;
}
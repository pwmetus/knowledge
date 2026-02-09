void main()
{
    int originSize = 20000;
    char* originBuffer;
    originBuffer = (char*)malloc(originSize);
    
    unsigned char *gzip_data = NULL;
    unsigned long gzip_len = 0;
    
    if (compress_to_gzip(originBuffer, originSize, &gzip_data, &gzip_len) != 0)
    {
        printf("압축실패\n");
        return;
    }
    
    unsigned char *decompBuffer;
    unsigned long decompLen;
    
    if (decompress_to_gzip(gzip_data, gzip_data, &decompBuffer, &decompLen) != 0)
    {
        printf("압축해제 실패\n");
        return;
    }
    
    free(originBuffer);
    free(gzip_data); // 중요
    free(decompBuffer); // 중요
}

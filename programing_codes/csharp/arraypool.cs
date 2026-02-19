public void SafeThreadWork(int size)
{
    // 1. 공유 풀에서 버퍼 대여 (GC 부하 감소)
    byte[] buffer = ArrayPool<byte>.Shared.Rent(size);

    try
    {
        // 2. Span으로 변환하여 안전하고 빠르게 작업
        Span<byte> span = buffer.AsSpan(0, size);
        DoInternalWork(span);
    }
    finally
    {
        // 3. 반드시 반납하여 메모리 누수 방지
        ArrayPool<byte>.Shared.Return(buffer);
    }
}
public abstract class ObservableObjectEx : ObservableObject
{
    private static readonly ConcurrentDictionary<string, PropertyInfo> _propertyCache = new();
    protected bool SetPropertyEx<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrEmpty(propertyName)) return false;

        // 1. 현재 UI 스레드인 경우: 즉시 값 변경 및 이벤트 발생 (기존 로직)
        if (App.Current.Dispatcher.CheckAccess())
        {
            return SetProperty(ref field, value, propertyName);
        }
        else
        {
            // 2. 작업 스레드인 경우: UI 스레드 재할당 예약
            // ref field 람다 캡처가 불가능하므로, Property 이름을 통해 재접근함
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                var type = this.GetType();
                var key = $"{type.FullName}.{propertyName}";

                // 캐시에서 PropertyInfo 조회 또는 생성
                var property = _propertyCache.GetOrAdd(key, _ => type.GetProperty(propertyName) 
                    ?? throw new InvalidOperationException($"Property '{propertyName}' not found in type '{type.FullName}'"));

                if (property != null && property.CanWrite)
                {
                    // UI 스레드에서 Setter 호출 -> 다시 이 메서드가 호출되지만 CheckAccess가 true가 되어 종료됨
                    property.SetValue(this, value);
                }
            });

            // 비동기 처리이므로 일단 true 반환 (UI는 곧 업데이트될 예정)
            return true; 
        }
    }
    
}
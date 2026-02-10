public class SomeA
{
    private List<string> _items;
}

public class Tester
{
    [Fact]
    public SomeA_Reflaction_Field()
    {
        var someInst = new SomeA();
        var field = typeof(SomeA).GetField("_items", 
                    System.Reflection.BindingFlags.NonPublic | 
                    System.Reflection.BindingFlags.Instance);

        // object 타입을 List<string>으로 캐스팅
        var list = field.GetValue(someInst) as List<string>;

        list.Add("Some Item");
    }
}

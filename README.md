# unity-reactive-hooks

A lightweight script for Unity that streamlines variable management in UI development, similar to React Hooks.


<br/>
 
### Example

When the RefType variable in ExampleInventoryClass is modified within ExampleGameComponent, all connected UI elements are updated automatically, eliminating the need for manual checks in user code

```cs

static class ExampleInventoryClass
{
    public static RefType<string> UserName = new("DummyCaptain");
    public static RefType<int> GoldCoin = new();
}

class ExampleUI : MonoBehaviour
{
    public TextMeshProUGUI txtUserGoldCoint;
    public TextMeshProUGUI txtUserName;

    void Start()
    {
        // Subscribe to variable changes.
        ExampleInventoryClass.UserName[this] = (value) => txtUserName.text = value;
        ExampleInventoryClass.GoldCoin[this] = (value) => txtUserGoldCoint.text = "Gold: " + value;
    }
}

class ExampleGameComponent : MonoBehaviour
{
    void Update()
    {
        // Increment GoldCoin value triggers UI updates.
        ExampleInventoryClass.GoldCoin.Value++;
    }
}
```

<br/>

### Quick QA

#### What happens when another scene updates the variable?
Callbacks from the previous scene are automatically cleared when a new scene loads thus eliminating cross-scene update.

#### Is RefTypes are thread safe?
RefTypes are not thread-safe. However, they are safe to use within coroutines.

#### How fast is it?
The performance of the RefTypes usually suited for many UI tasks altough calls happening under the 0 ms there is room to improve it much further.
 <br />
| Operation  | Execution Time |
| ------------- | ------------- |
| RefType Assignment  | 1270 Ticks ( 0 < ms) |
| Direct Assignment  | 7 Ticks ( 0 < ms) |

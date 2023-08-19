<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/sangeethnandakumar/Twileloop.SST">
    <img src="https://iili.io/HtlLJVV.png" alt="Logo" width="80" height="80">
  </a>

  <h1 align="center"> Twileloop.SST </h1>
  <small>(Single Source Of Truth)</small>
  <h4 align="center"> Centralised | State Management | Time Travel </h4>
</div>

## About
`Twileloop.SST` is a lightweight implementation of a single-source-of-truth concept. 

This package allows you to store your application state (data required for running your app / keeping a session) centrally in memory as a singleton instance and surround it with state management functions.
Once a state is stored, Your app should stick to this state (single-source-of-truth), Allowing you to make the app respond predictably as the state changes.

<hr/>

You can modify this state from anywhere in your app,

Get callbacks when your state changes, see state update history, diff 2 states, or even implement time travel with state undo & redo.
## License
> Twileloop.SST - is licensed under the MIT License. See the LICENSE file for more details.

#### This library is absolutely free. If it gives you a smile, A small coffee would be a great way to support my work. Thank you for considering it!
[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/sangeethnanda)

## Usage

## 2. Install Package

```powershell
dotnet add package Twileloop.SST
```

### Supported Features

### PLEASE NOTE
✅ - `Twileloop.SST` depends on `ObjectComparator` to do object diffing operations. Deep clones uses serialization techniques from `System.Text.Json`

| Feature     | Status 
| ---      | ---
| Central Memory Store (As Singleton) | ✅
| Undo/Redo State | ✅
| Clear State | ✅
| Class Type - State Model | ✅
| Record Type - State Model | ❌
| Read/Write State | ✅
| State History Tracking | ✅
| State Diffing (2 States) | ✅
| State Clonning | ✅


✅ - Available &nbsp;&nbsp;&nbsp; 
🚧 - Work In Progress &nbsp;&nbsp;&nbsp; 
❌ - Not Available

## 1. Declare your app state (POCO)
```csharp
    public class MyAppState
    {
        public int CurrentUserId { get; set; }
        public string CurrentUserName { get; set; }
        public string CurrentUserFName { get; set; }
        public string CurrentUserLName { get; set; }
        public DateTime CurrentUserDOB { get; set; }
    }
```

#### PLEASE NOTE :
⚠️⚠️ Warning  ⚠️⚠️

`Twileloop.SST` doesn't support the `record` type as your State model.
This is because diffing using `ObjectComparator` uses the default EqualityComparer which is for `records` compares values, over the reference if using a `class`.
This impacts the diffing processes.

You can still consider using `record` types. If you are not looking for diffing or accessing state history. Invoking diffing or history will still not throw any exception but returns full object without detecting any changes |


## 2. Initialize a global Session<T>, Wherever you need
```csharp
private SST<MyAppState> session = SST<MyAppState>.Instance;
```

## 3. Register any callbacks if you need (optional)
```csharp
//Step 1: Register for an update callback
session.RegisterStateUpdateCallback(Notify);
void Notify(MyAppState state)
{
    Console.WriteLine("My State Updated");
}
```

## 4. Set State
```csharp
//Step 2: Set your state
session.SetState(state =>
{
    state.CurrentUserId = 1;
    state.CurrentUserName = "sangee";
    state.CurrentUserFName = "Sangeeth";
    state.CurrentUserLName = "Nandakumar";
    state.CurrentUserDOB = DateTime.Now;
});
```

## 5. Get State
```csharp
//Step 3: Get your state
var myState = session.State;
Console.WriteLine(JsonConvert.SerializeObject(myState, Formatting.Indented));
```

## 6. Get All Your State History
```csharp
//Step 5: See your state update history
foreach (MyAppState history in session.GetStateHistory())
{
    Console.WriteLine("_____________________________________________________________");
    Console.WriteLine(JsonConvert.SerializeObject(history, Formatting.Indented));
}
```

## 7. Diff 2 States From History Or Wherever
```csharp
//Step 6: Compare 2 states
var newState = session.GetStateHistory().LastOrDefault();
var oldState = session.GetStateHistory().FirstOrDefault();
var diff = session.CompareStates(newState, oldState);
Console.WriteLine(JsonConvert.SerializeObject(diff, Formatting.Indented));
```

## 8. Undo/Redo Application State
```csharp
//Step 7: Undo/Redo states
session.Undo();
Console.WriteLine(JsonConvert.SerializeObject(session.State, Formatting.Indented));
session.Redo();
Console.WriteLine(JsonConvert.SerializeObject(session.State, Formatting.Indented));
```

## 9. Deep clone as a value copy with no reference to state (Useful if you want to mutate for any use)
```csharp
//Step 7: Get a value copy of your state. Useful if your state is a class
var clone = session.DeepClone();
```

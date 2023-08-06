using Newtonsoft.Json;
using Twileloop.SST.Demo;
using Twileloop.SST;

//Initialize a session. This will create a new State<T> inside it
var session = SST<MyAppState>.Instance;

//FEATURES

//Step 1: Register for an update callback
session.RegisterStateUpdateCallback(Notify);
void Notify(MyAppState state)
{
    Console.WriteLine("My State Updated");
}

//Step 2: Set your state
session.SetState(state =>
{
    state.CurrentUserId = 1;
    state.CurrentUserName = "sangee";
    state.CurrentUserFName = "Sangeeth";
    state.CurrentUserLName = "Nandakumar";
    state.CurrentUserDOB = DateTime.Now;
});

//Step 3: Get your state
Console.WriteLine(JsonConvert.SerializeObject(session.State, Formatting.Indented));

//Step 4: Modify your state/SST from anywhere
new OtherService().ModifyState();
//Read values
Console.WriteLine(JsonConvert.SerializeObject(session.State, Formatting.Indented));

//Step 5: See your state update history
foreach (MyAppState history in session.GetStateHistory())
{
    Console.WriteLine("_____________________________________________________________");
    Console.WriteLine(JsonConvert.SerializeObject(history, Formatting.Indented));
}

//Step 6: Compare 2 states
var newState = session.GetStateHistory().LastOrDefault();
var oldState = session.GetStateHistory().FirstOrDefault();
var diff = session.CompareStates(newState, oldState);
Console.WriteLine(JsonConvert.SerializeObject(diff, Formatting.Indented));

//Step 7: Undo/Redo states
session.Undo();
Console.WriteLine(JsonConvert.SerializeObject(session.State, Formatting.Indented));
session.Redo();
Console.WriteLine(JsonConvert.SerializeObject(session.State, Formatting.Indented));

//Step 7: Get a value copy of your state. Usefull if your state is a class
var clone = session.DeepClone();
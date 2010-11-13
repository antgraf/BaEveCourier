using NUnit.Framework;
using Courier.States;
using ExecutionActors;
using System.Threading;

namespace Courier.Tests
{
	[TestFixture]
	public class CourierStateMachineTest
	{
		private static void WaitForEvent()
		{
			const int heartBeatInterval = 5000 + 1;
			Thread.Sleep(heartBeatInterval);
		}

		[Test]
		public void HighLevelStatesTest()
		{
			CourierStateMachine m = new CourierStateMachine();
			StateMachine.UnRegisterAll();
			StateMachine.Register(CourierStateMachine.Id, m);
			Assert.NotNull(m);
			Assert.AreEqual(typeof(IdleState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.Start);
			WaitForEvent();
			Assert.AreEqual(typeof(WorkState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.Sleep);
			WaitForEvent();
			Assert.AreEqual(typeof(SleepState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.Start);
			WaitForEvent();
			Assert.AreEqual(typeof(WorkState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.End);
			WaitForEvent();
			Assert.AreEqual(typeof(IdleState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.Sleep);
			WaitForEvent();
			Assert.AreEqual(typeof(SleepState), m.CurrentSubState.GetType());
		}
	}
}

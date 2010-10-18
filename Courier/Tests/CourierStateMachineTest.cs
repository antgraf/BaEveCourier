using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Courier.States;
using ExecutionActors;

namespace Courier.Tests
{
	[TestFixture]
	public class CourierStateMachineTest
	{
		[Test]
		public void HighLevelStatesTest()
		{
			CourierStateMachine m = new CourierStateMachine();
			StateMachine.Register(CourierStateMachine.Id, m);
			Assert.NotNull(m);
			Assert.AreEqual(typeof(IdleState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.Start);
			Assert.AreEqual(typeof(WorkState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.Sleep);
			Assert.AreEqual(typeof(SleepState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.Start);
			Assert.AreEqual(typeof(WorkState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.End);
			Assert.AreEqual(typeof(IdleState), m.CurrentSubState.GetType());
			m.HandleEvent(CourierEvents.Sleep);
			Assert.AreEqual(typeof(SleepState), m.CurrentSubState.GetType());
		}
	}
}

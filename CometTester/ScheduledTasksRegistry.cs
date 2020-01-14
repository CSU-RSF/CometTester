﻿using System;
﻿using FluentScheduler;
using CometTester;


public class ScheduledTasksRegistry : Registry
{
    public ScheduledTasksRegistry(string hour, string min, MainWindow mW)
    {

        Action someMethod = new Action(() =>
        {
            mW.RunScheduledTest();
        });

        Schedule(someMethod).ToRunEvery(1).Days().At(Int16.Parse(hour),Int16.Parse(min)); 
    }
}
using System;
using System.Collections;

public interface IExecuteIntention
{
    IEnumerator ExecuteIntention(Action finishCallback);
}
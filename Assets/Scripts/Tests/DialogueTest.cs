using NUnit.Framework;

public class DialogueTest
{
    // A Test behaves as an ordinary method
    Line[] Lines = new Line[]{new Line("Grave Robber", "second", -1), new Line("Grave Robber", "second", -1), new Line("Grave Robber", "third", -1)};
    Dialogue _Dialogue = new Dialogue();

    
    [Test]
    public void TestLineBehavior()
    {
        //Normal constructor test
        Line line1 = new Line("Grave Robber", "Word go here", 2);
        Assert.AreEqual(line1.Title, "Grave Robber");
        Assert.AreEqual(line1.ScriptLine, "Word go here");
        Assert.AreEqual(line1.Portrait, 2);

        //Check that the other constructor that takes a line also works
        Line line2 = new Line(line1);
        Assert.AreEqual(line2.Title, "Grave Robber");
        Assert.AreEqual(line2.ScriptLine, "Word go here");
        Assert.AreEqual(line2.Portrait, 2);

        //Test that the big setter for all values works
        line1.SetLine("Crab", "Words", 3);
        Assert.AreEqual(line1.Title, "Crab");
        Assert.AreEqual(line1.ScriptLine, "Words");
        Assert.AreEqual(line1.Portrait, 3);

        //Test that the setter that takes in a line works
        line2.SetLine(line1);
        Assert.AreEqual(line2.Title, "Crab");
        Assert.AreEqual(line2.ScriptLine, "Words");
        Assert.AreEqual(line2.Portrait, 3);
    }

    [Test]
    public void GetLine()
    {
        Script DialogueScript = Script.CreateInstance<Script>();
        DialogueScript.Add(Lines);
        //Test to make sure the getLine will get the line and if past end then return last element
        Assert.AreEqual(Lines[1], DialogueScript.GetLine(1));
    }

    [Test]
    public void GetRandomLine()
    {
        Script DialogueScript = Script.CreateInstance<Script>();
        DialogueScript.Add(Lines);
        //Prove that the random line is returned as a line
        Line result = DialogueScript.GetRandomLine();
        Assert.AreEqual(typeof(Line), result.GetType());
        //Prove that the random line is non empty
        Assert.AreEqual("Grave Robber", result.Title);
    }

}

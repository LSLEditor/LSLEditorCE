// www.lsleditor.org  by Alphons van der Heijden (SL: Alphons Jano)
default
{
	state_entry()
	{
		llSay(0, "Hello, Avatar!");
	}
	touch_start(integer total_number)
	{
		llSay(0, "Touched: "+(string)total_number);
		llOwnerSay(llGetScriptName());
	}
}
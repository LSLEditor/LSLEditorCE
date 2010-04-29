// www.lsleditor.org  by Alphons van der Heijden (SL: Alphons Jano)
default
{
	state_entry()
	{
		llSay(0, "Ready to test!");
	}
	touch_start(integer total_number)
	{
		llSay(0, "Length of \\t (tab) character: "+(string)llStringLength("\t"));
	}
}

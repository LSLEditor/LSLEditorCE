
default
{
	state_entry()
	{
		llSay(0, (string)PRIM_NAME);
	}
	touch_start(integer total_number)
	{
		llSay(0, "Touched: "+(string)total_number);
	}
}


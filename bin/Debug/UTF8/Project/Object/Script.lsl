string card;
integer lines = -1;
integer line = 0;
list resuts;
integer pass;
integer fail;

default
{
	state_entry()
	{
		if(llGetInventoryNumber(INVENTORY_NOTECARD))
		{
			llGetNumberOfNotecardLines(
				card = llGetInventoryName(INVENTORY_NOTECARD,0));
			llOwnerSay("Test-Starting: "+card);
		}
	}
	on_rez(integer a)
	{
		llResetScript();
	}
	touch_start(integer a)
	{
		llResetScript();
	}
	changed(integer a)
	{
		if(a & CHANGED_INVENTORY)
			llResetScript();
	}

	dataserver(key a, string b)
	{
		if(lines == -1)
		{
			lines = (integer)b;
			llGetNotecardLine(card,line);
		}
		else
		{
			list c = llParseString2List(a=(string)llParseString2List(b,[" "],[]),["|"],[EOF]);
			integer d;
			integer e;
			integer f;
			if(llGetSubString(a,0,0) == "#")
				llOwnerSay(llDeleteSubString(b,0,0));
			else if(llGetListLength(c) >= 2)
			{
				d = llStringLength(b = llUnescapeURL(llList2String(c,0)));
				f = llList2Integer(c,1);
				e = (d == f);
				pass += e;
				fail += !e;
				string out = (string)line +": ";
				out += llList2String(["Fail","Pass"],e) + " ";
				out += "(" + (string)d + " - " + (string)f + ") ";
				//Enable this section to test llUnescapeURL
				//                       out += "(" + (string)((
				//                                llStringLength(
				//                                    (string)llParseString2List( //strips off the evil pad
				//                                        llStringToBase64(b),["="],[]
				//                                    )
				//                                ) * 3 ) / 4); //thats how many bytes should be in it (assuming all escaped)
				//                       out += " - ";
				//                       out += (string)(llStringLength(llList2String(c,0))/3) + ")";


				// This will IM all the tests to the owner!  This is slow because IM sleeps the script for 2 seconds.
				llInstantMessage(llGetOwner(),b);
				llOwnerSay(out);
			}
			if(llListFindList(c,[EOF]) == -1 && ++line < lines)
				llGetNotecardLine(card,line);
			else
			{
				llOwnerSay("Finished");
				llOwnerSay("Passed: "+(string)pass);
				llOwnerSay("Failed: "+(string)fail);
			}
		}
	}
}

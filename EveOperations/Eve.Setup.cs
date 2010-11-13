using WindowEntity;

namespace EveOperations
{
	public partial class Eve
	{
		private void SetOptions()
		{
			// POINT: video checkboxes
			Coordinate chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.298865069356873 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.32156368221942 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.346784363177806 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.387378640776699, Y = 0.368221941992434 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.390920554854981 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.413619167717528 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.466582597730139 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.489281210592686 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.534678436317781 });
			pEveWindow.LeftClick(chkbox);
			WaitRandom();
			// POINT: general settings tab
			Coordinate general = new Coordinate(
				new StretchedPoint() { X = 0.353398058252427, Y = 0.243379571248424 });
			pEveWindow.LeftClick(general);
			WaitRandom();
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.301387137452711 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.32156368221942 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.398486759142497 });
			pEveWindow.LeftClick(chkbox);
			// POINT: theme selector
			Coordinate theme = new Coordinate(
				new StretchedPoint() { X = 0.866990291262136, Y = 0.358133669609079 });
			// POINT: black theme
			Coordinate black = new Coordinate(
				new StretchedPoint() { X = 0.779611650485437, Y = 0.413619167717528 });
			pEveWindow.LeftClick(theme);
			WaitRandom();
			pEveWindow.LeftClick(black);
			WaitRandom();
			// POINT: color sliders
			Coordinate color = new Coordinate(
				new StretchedPoint() { X = 0.842718446601942, Y = 0.442622950819672 });
			// POINT: color sliders dragging target point
			Coordinate dragto = new Coordinate(
				new StretchedPoint() { X = 0.885436893203884, Y = 0.443883984867591 });
			pEveWindow.DragDrop(color, dragto);
			color = new Coordinate(
				new StretchedPoint() { X = 0.809708737864078, Y = 0.532156368221942 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.733009708737864, Y = 0.532156368221942 });
			pEveWindow.DragDrop(color, dragto);
			color = new Coordinate(
				new StretchedPoint() { X = 0.810679611650485, Y = 0.549810844892812 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.736893203883495, Y = 0.551071878940731 });
			pEveWindow.DragDrop(color, dragto);
			color = new Coordinate(
				new StretchedPoint() { X = 0.809708737864078, Y = 0.563682219419924 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.735922330097087, Y = 0.567465321563682 });
			pEveWindow.DragDrop(color, dragto);
			color = new Coordinate(
				new StretchedPoint() { X = 0.809708737864078, Y = 0.585119798234552 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.885436893203884, Y = 0.585119798234552 });
			pEveWindow.DragDrop(color, dragto);
			theme = new Coordinate(
				new StretchedPoint() { X = 0.867961165048544, Y = 0.640605296343001 });
			black = new Coordinate(
				new StretchedPoint() { X = 0.772815533980583, Y = 0.692307692307692 });
			pEveWindow.LeftClick(theme);
			WaitRandom();
			pEveWindow.LeftClick(black);
			WaitRandom();
			theme = new Coordinate(
				new StretchedPoint() { X = 0.867961165048544, Y = 0.673392181588903 });
			black = new Coordinate(
				new StretchedPoint() { X = 0.772815533980583, Y = 0.721311475409836 });
			pEveWindow.LeftClick(theme);
			WaitRandom();
			pEveWindow.LeftClick(black);
			WaitRandom();
			// POINT: shortcuts settings tab
			Coordinate shortcuts = new Coordinate(
				new StretchedPoint() { X = 0.430097087378641, Y = 0.247162673392182 });
			pEveWindow.LeftClick(shortcuts);
			WaitRandom();
			// POINT: shortcuts list
			Coordinate list = new Coordinate(
				new StretchedPoint() { X = 0.650485436893204, Y = 0.300126103404792 });
			pEveWindow.LeftClick(list);
			WaitRandom();
			for(int i = 0; i < 32; i++)	// go to "open active ship's cargo" shortcut
			{
				pEveWindow.KeySend("{DOWN}");
			}
			WaitRandom();
			// POINT: add shortcut button
			Coordinate addbutton = new Coordinate(
				new StretchedPoint() { X = 0.609708737864078, Y = 0.412358133669609 });
			pEveWindow.LeftClick(addbutton);
			WaitRandom();
			// POINT: alt checkbox in shortcut window
			Coordinate altbutton = new Coordinate(
				new StretchedPoint() { X = 0.394174757281553, Y = 0.501891551071879 });
			pEveWindow.LeftClick(altbutton);
			WaitRandom();
			// POINT: textbox in shortcut window
			Coordinate textbox = new Coordinate(
				new StretchedPoint() { X = 0.446601941747573, Y = 0.549810844892812 });
			pEveWindow.LeftClick(textbox);
			WaitRandom();
			pEveWindow.KeySendAndWait("c", pStandardWaitTime);	// ctrl+alt+c for open cargo
			// POINT: ok button shortcut window
			Coordinate shortcutok = new Coordinate(
				new StretchedPoint() { X = 0.448543689320388, Y = 0.601513240857503 });
			pEveWindow.LeftClick(shortcutok);
			WaitRandom();
		}
	}
}

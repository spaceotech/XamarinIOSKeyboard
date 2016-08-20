// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace KeyBoardHandling_Xamarin_iOS
{
	[Register ("KBHandling")]
	partial class KBHandling
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtField1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtField2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtField3 { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (txtField1 != null) {
				txtField1.Dispose ();
				txtField1 = null;
			}
			if (txtField2 != null) {
				txtField2.Dispose ();
				txtField2 = null;
			}
			if (txtField3 != null) {
				txtField3.Dispose ();
				txtField3 = null;
			}
		}
	}
}

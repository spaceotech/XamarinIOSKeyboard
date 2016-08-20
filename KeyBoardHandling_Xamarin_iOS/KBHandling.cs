
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace KeyBoardHandling_Xamarin_iOS
{
    public class NextPreviousToolBar : UIToolbar
    {
        public UIView prevTextField { get; set; }
        public UIView currentTextField { get; set; }
        public UIView nextTextField { get; set; }

        public NextPreviousToolBar() : base() { }

        public NextPreviousToolBar(UIView curr, UIView prev,
            UIView next)
        {
            this.currentTextField = curr;
            this.prevTextField = prev;
            this.nextTextField = next;
            AddButtonsToToolBar();
        }

        void AddButtonsToToolBar()
        {
            Frame = new CoreGraphics.CGRect(0.0f, 0.0f, 320, 44.0f);
            TintColor = UIColor.DarkGray;
            Translucent = false;
            Items = new UIBarButtonItem[]
            {
            new UIBarButtonItem("Prev",
                UIBarButtonItemStyle.Bordered, delegate
                {
                    prevTextField.BecomeFirstResponder();
                }) { Enabled = prevTextField != null },
            new UIBarButtonItem("Next",
                UIBarButtonItemStyle.Bordered, delegate
                {
                    nextTextField.BecomeFirstResponder();
                }) { Enabled = nextTextField != null },
        new
           UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
        new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
        {
            currentTextField.ResignFirstResponder();
        })
            };
        }
    }

    public partial class KBHandling : UIViewController
    {
        private NSObject _didShowNotificationObserver;
        private NSObject _willHideNotificationObserver;
        private UIView activeTextFieldView;
        private nfloat amountToScroll = 0.0f;
        private nfloat alreadyScrolledAmount = 0.0f;
        private nfloat bottomOfTheActiveTextField = 0.0f;
        private nfloat offsetBetweenKeybordAndTextField = 10.0f;
        private bool isMoveRequired = false;

        public KBHandling(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            #region Move UI View Up Handling
            // Keyboard popup
            _didShowNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.DidShowNotification, KeyBoardDidShow, this);

            // Keyboard Down
            _willHideNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.WillHideNotification, KeyBoardWillHide, this);
            #endregion


            #region Include Return Button in Keyboard
            this.txtField1.ShouldReturn += (textField) =>
            {
                txtField1.ResignFirstResponder();
                return true;
            };

            this.txtField2.ShouldReturn += (textField) =>
            {
                txtField2.ResignFirstResponder();
                return true;
            };

            this.txtField3.ShouldReturn += (textField) =>
            {
                txtField3.ResignFirstResponder();
                return true;
            };
            #endregion

            #region When Clicked out side keyboard, Close the Keyboard
            UITapGestureRecognizer g_recognizer = new UITapGestureRecognizer(() =>
            {
                txtField1.ResignFirstResponder();
                txtField2.ResignFirstResponder();
                txtField3.ResignFirstResponder();
            });
            this.View.AddGestureRecognizer(g_recognizer);
            #endregion

            #region Add Next Previous Buttons to Toolbar
            txtField1.InputAccessoryView = new NextPreviousToolBar(txtField1, null, txtField2);
            txtField2.InputAccessoryView = new NextPreviousToolBar(txtField2, txtField1, txtField3);
            txtField3.InputAccessoryView = new NextPreviousToolBar(txtField3, txtField2, null);
            #endregion

        }



        #region Move UI View Up Handling
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _didShowNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyBoardDidShow);

            _willHideNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardWillHide);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (_didShowNotificationObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_didShowNotificationObserver);
            }

            if (_willHideNotificationObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_willHideNotificationObserver);
            }
        }


        private void KeyBoardDidShow(NSNotification notification)
        {
            // get the keyboard size
            CoreGraphics.CGRect notificationBounds = UIKeyboard.BoundsFromNotification(notification);

            // Find what opened the keyboard
            foreach (UIView view in this.View.Subviews)
            {
                if (view.IsFirstResponder)
                    activeTextFieldView = view;
            }

            // Bottom of the controller = initial position + height + offset      
            bottomOfTheActiveTextField = (activeTextFieldView.Frame.Y + activeTextFieldView.Frame.Height + offsetBetweenKeybordAndTextField);

            // Calculate how far we need to scroll
            amountToScroll = (notificationBounds.Height - (View.Frame.Size.Height - bottomOfTheActiveTextField));

            // Perform the scrolling
            if (amountToScroll > 0)
            {
                bottomOfTheActiveTextField -= alreadyScrolledAmount;
                amountToScroll = (notificationBounds.Height - (View.Frame.Size.Height - bottomOfTheActiveTextField));
                alreadyScrolledAmount += amountToScroll;
                isMoveRequired = true;
                ScrollTheView(isMoveRequired);
            }
            else
            {
                isMoveRequired = false;
            }

        }

        private void KeyBoardWillHide(NSNotification notification)
        {
            bool wasViewMoved = !isMoveRequired;
            if (isMoveRequired) { ScrollTheView(wasViewMoved); }
        }

        private void ScrollTheView(bool move)
        {

            // scroll the view up or down
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);

            CoreGraphics.CGRect frame = View.Frame;

            if (move)
            {
                frame.Y -= amountToScroll;
            }
            else
            {
                frame.Y += alreadyScrolledAmount;
                amountToScroll = 0;
                alreadyScrolledAmount = 0;
            }

            View.Frame = frame;
            UIView.CommitAnimations();
        }
        #endregion

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }


        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
    }
}
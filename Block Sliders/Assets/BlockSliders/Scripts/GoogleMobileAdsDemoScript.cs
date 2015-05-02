using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsDemoScript : MonoBehaviour
{
		private string adUnitId;
		private BannerView bannerView;		
		private bool showing;
	
		void Start ()
		{
				bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Top);	
				adUnitId = "ca-app-pub-2255839828586145/5498178517";
				Setup ();
		}
		
		public void ShowAd (bool show)
		{
				if (show == true) {
						bannerView.Show ();
						showing = true;
				} else {
						bannerView.Hide ();
						showing = false;
						bannerView.Destroy ();
				}
		}
		
		void Update ()
		{

				if (Application.loadedLevel <= 1 && !showing) {
						bannerView.Show ();
						Setup ();
						RequestBanner ();
						ShowAd (true);
						showing = true;
				} else if (Application.loadedLevel >= 2 && showing) {
						bannerView.Hide ();
						ShowAd (false);
						bannerView.Destroy ();
						showing = false;
				}
		}
		
		private void Setup ()
		{
		
				showing = true;
				bannerView.AdLoaded += HandleAdLoaded;
				bannerView.AdFailedToLoad += HandleAdFailedToLoad;
				bannerView.AdOpened += HandleAdOpened;
				bannerView.AdClosing += HandleAdClosing;
				bannerView.AdClosed += HandleAdClosed;
				bannerView.AdLeftApplication += HandleAdLeftApplication;			
				RequestBanner ();			
				ShowAd (true);
		}
	
		private void RequestBanner ()
		{
				// Register for ad events.
				bannerView.AdLoaded += HandleAdLoaded;
				bannerView.AdFailedToLoad += HandleAdFailedToLoad;
				bannerView.AdOpened += HandleAdOpened;
				bannerView.AdClosing += HandleAdClosing;
				bannerView.AdClosed += HandleAdClosed;
				bannerView.AdLeftApplication += HandleAdLeftApplication;
				// Load a banner ad.
				bannerView.LoadAd (createAdRequest ());
				ShowAd (true);
		}
	
	
		// Returns an ad request with custom ad targeting.
		private AdRequest createAdRequest ()
		{
				return new AdRequest.Builder ()
			.AddTestDevice (AdRequest.TestDeviceSimulator)
				.AddTestDevice ("53A3E2583A7F53AE0682D297CD96907C")
				.AddKeyword ("game")
				.SetGender (Gender.Male)
				.SetBirthday (new DateTime (1985, 1, 1))
				.TagForChildDirectedTreatment (false)
				.AddExtra ("color_bg", "9B30FF")
				.Build ();
		
		}
	
	#region Banner callback handlers
	
		public void HandleAdLoaded (object sender, EventArgs args)
		{
				print ("HandleAdLoaded event received.");
		}
	
		public void HandleAdFailedToLoad (object sender, AdFailedToLoadEventArgs args)
		{
				print ("HandleFailedToReceiveAd event received with message: " + args.Message);
		}
	
		public void HandleAdOpened (object sender, EventArgs args)
		{
				print ("HandleAdOpened event received");
		}
	
		void HandleAdClosing (object sender, EventArgs args)
		{
				print ("HandleAdClosing event received");
		}
	
		public void HandleAdClosed (object sender, EventArgs args)
		{
				print ("HandleAdClosed event received");
		}
	
		public void HandleAdLeftApplication (object sender, EventArgs args)
		{
				print ("HandleAdLeftApplication event received");
		}
	
	#endregion
}
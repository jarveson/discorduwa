using System;
using Windows.UI.Xaml;

namespace DiscordUWA.Common {
    public class Throttler {
        private DispatcherTimer _timer;
        private object _instanceLock = new object(); 
        private bool shouldTrigger = true;

        public Throttler(TimeSpan timeSpan) {
            _timer = new DispatcherTimer() { Interval = timeSpan };
            _timer.Tick += Timer_Tick;
        }
        
        public event RoutedEventHandler Action;
        
        private void Timer_Tick(object sender, object e) {
            _timer.Stop();
            lock(_instanceLock) {
                shouldTrigger = true;
            }
        }
        
        public void ResetAndTick() {
            _timer.Start();
            lock(_instanceLock) {
                if (shouldTrigger) {
                    if (Action != null)
                        Action(this, new RoutedEventArgs());
                }
                shouldTrigger = false;
            }
        }
        
        public void Stop() {
            _timer.Stop();
        }
    }
}
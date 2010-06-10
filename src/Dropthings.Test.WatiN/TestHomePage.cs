﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSpec;
using Dropthings.Test.WatiN.Pages;
using System.Threading;
using WatiN.Core;
using Xunit;
using Dropthings.Test.WatiN.Controls;
using System.Text.RegularExpressions;
using System.Security.Permissions;
using System.Collections.Specialized;

namespace Dropthings.Test.WatiN
{    
    class TestHomePage
    {
        [Specification]
        public void Visit_Homepage_as_new_user_and_verify_default_widgets()
        {
            var browser = default(Browser);

            "Given a new user".Context(() =>
                {
                    BrowserHelper.ClearCookies();
                });
            "When user visits the homepage".Do(() =>
                {
                    browser = BrowserHelper.OpenNewBrowser(Urls.Homepage);
                });
            "It should show the default widgets".Assert(() =>
                {
                    using (browser)
                    {
                        var expectedWidgets = new string[] 
                        {
                            "How to of the Day",
                            "Omar's Blog (Fast RSS)",
                            "Book on building Dropthings",
                            "Twitter",
                            "Fast Flickr",
                            "Digg - Silverlight Widget",
                            "BBC World",
                            "Weather",
                            "CNN.com",
                            "Travelocity",
                            "Stock",
                            "HTML"
                        };

                        var homepage = browser.Page<HomePage>();

                        Assert.Equal(expectedWidgets.Length, homepage.Widgets.Count);

                        Assert.True(
                            expectedWidgets.All(
                                widgetTitle =>
                                    homepage.Widgets.Any(
                                        control => string.Compare(control.Title, widgetTitle, true) == 0)));
                    }
                });
        }

        [Specification]
        public void User_can_change_widget_title()
        {
            var browser = default(Browser);
            var widgetId = default(string);
            var newTitle = Guid.NewGuid().ToString();

            "Given a user with a page having some widgets".Context(() =>
            {
                BrowserHelper.ClearCookies();
                browser = BrowserHelper.OpenNewBrowser(Urls.Homepage);
            });
            "When user changes title of a widget".Do(() =>
            {
                using (browser)
                {
                    var page = browser.Page<HomePage>();
                    var widget = page.Widgets.First();
                    widgetId = widget.Element.Id;

                    widget.EditTitle();
                    widget.SetNewTitle(newTitle);

                    Thread.Sleep(1000);
                }
            });
            "It should persist the new title on next visit".Assert(() =>
            {
                using (browser = BrowserHelper.OpenNewBrowser(Urls.Homepage))
                {
                    var widget = browser.Control<WidgetControl>(widgetId);
                    Assert.Equal(newTitle, widget.Title);
                }
            });
        }

        [Specification]
        public void User_can_add_show_the_widget_gallery()
        {
            var browser = default(Browser);
            var page = default(HomePage);
            "Given a user on the homepage".Context(() =>
            {
                browser = BrowserHelper.OpenNewBrowser(Urls.Homepage);
                page = browser.Page<HomePage>();
            });

            "When user clicks on the 'Add Stuff' link".Do(() =>
            {
                page.ShowAddStuff();
                browser.WaitForAsyncPostbackComplete(10000);
            });

            "It should show the widget gallery".Assert(() =>
            {
                Assert.True(page.WidgetDataList.Exists);
                Assert.NotEqual(0, page.AddWidgetLinks.Count);
            });

            "Shutdown".Finally(() =>
            {
                browser.Dispose();
            });
        }

        [Specification]
        public void User_can_change_rss_widget_setting()
        {
            var browser = default(Browser);
            var page = default(HomePage);
            var rssWidget = default(RssWidgetControl);

            "Given a new user on the homepage having a RSS widget".Context(() =>
            {
                BrowserHelper.ClearCookies();
                browser = BrowserHelper.OpenNewBrowser(Urls.Homepage);
                browser.WaitForAsyncPostbackComplete(10000);

                page = browser.Page<HomePage>();
                rssWidget = RssWidgetControl.GetTheFirstRssWidget(page);
            });

            "When user clicks on the edit link of the widget".Do(() =>
            {                
                rssWidget.EditLink.Click();
                browser.WaitForAsyncPostbackComplete(10000);
            });

            "It should show the settings area of the widget".Assert(() =>
            {
                using (browser)
                {
                    Assert.True(rssWidget.UrlTextBox.Exists);
                    Assert.Equal("http://www.wikihow.com/feed.rss", rssWidget.UrlTextBox.Text);
                    Assert.Equal("3", rssWidget.CountDropdown.SelectedItem);
                }
            });

            "It should allow user to change the number of feeds to show".Assert(() =>
            {
                using (browser)
                {
                    rssWidget.CountDropdown.Select("10");
                    rssWidget.SaveButton.Click();

                    browser.WaitForAsyncPostbackComplete(10000);
                    Assert.Equal(10, rssWidget.FeedLinks.Count);                    
                }
            });

            "It should persist the changes on next page load".Assert(() =>
            {
                using (browser)
                {
                    // Make changes
                    rssWidget.CountDropdown.Select("5");
                    rssWidget.SaveButton.Click();

                    browser.WaitForAsyncPostbackComplete(10000);

                    // Reload the page
                    browser.GoTo(Urls.Homepage);
                    Thread.Sleep(5000);
                    browser.WaitForAsyncPostbackComplete(10000);

                    page = browser.Page<HomePage>();
                    rssWidget = RssWidgetControl.GetTheFirstRssWidget(page);

                    // Ensure the changes are reflected
                    Assert.Equal(5, rssWidget.FeedLinks.Count);
                }
            });

        }

        public void User_can_delete_widget()
        {
            
        }

        public void User_can_collapse_widget()
        {
        }

        public void User_can_maximize_widget()
        {
        }

        [Specification]
        public void User_can_drag_and_drop_widget()
        {
            var browser = default(Browser);
            var page = default(HomePage);
            var widget = default(WidgetControl);
            var originalColumn = default(Div);
            var newColumn = default(Div);

            BrowserHelper.ClearCookies();

            "Given a new user on a page full of widgets".Context(() =>
                {
                    browser = BrowserHelper.OpenNewBrowser(Urls.Homepage);
                    browser.WaitForAsyncPostbackComplete(10000);
                });
            "When user drags and drops a widget from first column to second column".Do(() =>
                {
                    page = browser.Page<HomePage>();
                    widget = page.Widgets.First();
                    originalColumn = widget.Element.Parent as Div;

                    // Start the drag
                    var mouseDownEvent = new NameValueCollection();
                    mouseDownEvent.Add("button", "1");
                    mouseDownEvent.Add("clientX", "0");
                    mouseDownEvent.Add("clientY", "0");
                    widget.Header.FireEventNoWait("onmousedown", mouseDownEvent);
                    
                    Thread.Sleep(500);

                    //for (var y = 10; y < 200; y += 20)
                    //{
                    //    var mouseMoveEvent = new NameValueCollection();
                    //    mouseMoveEvent.Add("button", "1");
                    //    mouseMoveEvent.Add("clientX", "100");
                    //    mouseMoveEvent.Add("clientY", y.ToString());

                    //    browser.NativeDocument.Body.FireEventNoWait("onmousemove", mouseMoveEvent);
                    //    Thread.Sleep(500);
                    //}

                    // Move to the second column widget zone container
                    var widgetZones = browser.Divs.Filter(Find.ByClass("widget_zone_container"));
                    var aWidgetZone = widgetZones[0];
                    var widthOfWidgetZone = int.Parse(aWidgetZone.GetAttributeValue("clientWidth"));

                    for (var x = 0; x <= (widthOfWidgetZone * 1.5); x += (widthOfWidgetZone / 4))
                    {
                        var eventProperties = new NameValueCollection();
                        eventProperties.Add("button", "1");
                        eventProperties.Add("clientX", x.ToString());
                        eventProperties.Add("clientY", "20");

                        //browser.NativeDocument.Body.FireEvent("onmousemove", eventProperties);
                        widget.Header.FireEventNoWait("onmousemove", eventProperties);
                        Thread.Sleep(500);
                        
                        //foreach (var widgetZone in widgetZones)
                        //    widgetZone.FireEventNoWait("onmousemove", eventProperties);
                    }


                    var mouseUpProperties = new NameValueCollection();
                    mouseUpProperties.Add("button", "1");
                    mouseUpProperties.Add("clientX", (widthOfWidgetZone * 1.5).ToString());
                    mouseUpProperties.Add("clientY", "20");
                    widget.Header.FireEventNoWait("onmouseup");
                    Thread.Sleep(500);
                });
            "It should move the widget to the second column".Assert(() =>
                {
                    newColumn = widget.Element.Parent as Div;
                    Assert.NotEqual(newColumn.Id, originalColumn.Id);
                });
            //"It should remain the widget on second column after page reload".Assert(() =>
            //    {
            //    });

        }
    }
}

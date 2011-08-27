using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace Shellscape.UI.VisualStyles {

	public static class ControlPanel {

		public const String ClassName = "CONTROLPANEL";

		public enum ControlPanelPart : int {
			NavigationPane = 1, // CPANEL_NAVIGATIONPANE = 1,
			ContentPane = 2, // CPANEL_CONTENTPANE = 2,
			NavigationPaneLabel = 3, // CPANEL_NAVIGATIONPANELABEL = 3,
			ContentPaneLabel = 4, // CPANEL_CONTENTPANELABEL = 4,
			Title = 5, // CPANEL_TITLE = 5,
			BodyText = 6, // CPANEL_BODYTEXT = 6,
			HelpLink = 7, // CPANEL_HELPLINK = 7,
			TaskLink = 8, // CPANEL_TASKLINK = 8,
			GroupText = 9, // CPANEL_GROUPTEXT = 9,
			ContentLink = 10, // CPANEL_CONTENTLINK = 10,
			SectionTitleLink = 11, // CPANEL_SECTIONTITLELINK = 11,
			LargeCommandArea = 12, // CPANEL_LARGECOMMANDAREA = 12,
			SmallCommandArea = 13, // CPANEL_SMALLCOMMANDAREA = 13,
			Button = 14, // CPANEL_BUTTON = 14,
			MessageText = 15, // CPANEL_MESSAGETEXT = 15,
			NavigationPaneLine = 16, // CPANEL_NAVIGATIONPANELINE = 16,
			ContentPaneLine = 17, // CPANEL_CONTENTPANELINE = 17,
			BannerArea = 18, // CPANEL_BANNERAREA = 18,
			BodyTitle = 19, // CPANEL_BODYTITLE = 19,
		};

		public enum ContentLinkState : int {
			Normal = 1, // CPHL_NORMAL
			Hot = 2, // CPHL_HOT
			Pressed = 3, // CPHL_PRESSED
			Disabled = 4 // CPHL_DISABLED
		};

		public enum HelpLinkState : int {
			Normal = 1, // CPHL_NORMAL
			Hot = 2, // CPHL_HOT
			Pressed = 3, // CPHL_PRESSED
			Disabled = 4 // CPHL_DISABLED
		};

		public enum SectionTitleLinkState : int {
			Normal = 1, // CPHL_NORMAL
			Hot = 2 // CPHL_HOT
		};

		public enum TaskLinkState : int {
			Normal = 1, // CPHL_NORMAL
			Hot = 2, // CPHL_HOT
			Pressed = 3, // CPHL_PRESSED
			Disabled = 4, // CPHL_DISABLED
			Page = 5 // CPTL_PAGE
		};

		public static VisualStyleElement GetElement(ControlPanelPart part, int state) {
			return GetElement(part, state, false);
		}

		public static VisualStyleElement GetElement(ControlPanelPart part, int state, Boolean style) {
			String className = String.Concat(ControlPanel.ClassName, style ? "STYLE" : String.Empty);

			return VisualStyleElement.CreateElement(className, (int)part, state);
		}

		public static VisualStyleRenderer GetRenderer(ControlPanelPart part, int state) {
			return GetRenderer(part, state, false);
		}

		public static VisualStyleRenderer GetRenderer(ControlPanelPart part, int state, Boolean style) {
			if (VisualStyleRenderer.IsSupported) {
				return new VisualStyleRenderer(ControlPanel.GetElement(part, state));
			}
			else {
				return null;
			}
		}
	}
}

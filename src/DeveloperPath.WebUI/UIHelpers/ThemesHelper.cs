using MudBlazor;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeveloperPath.WebUI.UIHelpers;

public class ThemesHelper
{
  private readonly Dictionary<string, MudTheme> _themes = new();

  public ImmutableDictionary<string, MudTheme> Themes { get => _themes.ToImmutableDictionary(); }

  public ThemesHelper()
  {
    InitThemes();
  }

  private void InitThemes()
  {
    string[] fontFamily = @"-apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,Noto Sans,Liberation Sans,sans-serif,Apple Color Emoji,Segoe UI Emoji,Segoe UI Symbol,Noto Color Emoji".Split(",");
    var typography = new Typography
    {
      Default = new Default
      {
        FontFamily = fontFamily,
        FontSize = "1rem",
        FontWeight = 350
      },
      H1 = new H1
      {
        FontSize = "220%"
      },
      H2 = new H2
      {
        FontSize = "200%"
      },
      H3 = new H3
      {
        FontSize = "150%",
        FontWeight = 300
      },
      H4 = new H4
      {
        FontSize = "110%",
        FontWeight = 300
      }
    };
    var layout = new LayoutProperties
    {
      AppbarHeight = "50px"
    };

    _themes.Add("ThemeLight",
      new MudTheme()
      {
        LayoutProperties = layout,
        Typography = typography,
        Palette = new Palette
        {
          Primary = Colors.Blue.Default,
          Background = "#fff",
          TextPrimary = "#424242",
          DrawerBackground = "#eee"
        }
      }
    );

    _themes.Add("ThemeDark",
      new MudTheme()
      {
        LayoutProperties = layout,
        Typography = typography,
        Palette = new Palette
        {
          Primary = Colors.DeepPurple.Default,
          Background = "#191d21",
          TextPrimary = "#fff",
          DrawerBackground = "#292c2f",
          DrawerText = "#fff",
          Surface = "#343a40",
          ActionDefault = "#e9ecef"
        }
      }
    );
  }
}
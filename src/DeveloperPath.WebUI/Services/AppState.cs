using DeveloperPath.WebUI.UIHelpers;
using MudBlazor;
using System;
using System.Collections.Generic;

namespace DeveloperPath.WebUI.Services;

public class AppState
{
  private readonly Dictionary<string, string> _values = new();
  private readonly ThemesHelper _themesHelper;
  private MudTheme _mudTheme = new();

  public event Action OnChange;

  public AppState(ThemesHelper themesHelper)
  {
    _themesHelper = themesHelper;
    _mudTheme = _themesHelper.Themes["ThemeLight"];
  }

  public MudTheme CurrentTheme
  {
    get => _mudTheme;
  }

  public List<string> Themes
  {
    get => new List<string>(_themesHelper.Themes.Keys);
  }

  public void SetTheme(string name)
  {
    if (name == null) return;

    _themesHelper.Themes.TryGetValue(name, out var theme);
    if (theme is null) return;

    Set("theme", name);
    _mudTheme = theme;
    NotifyStateChanged();
  }

  public void Set(string key, string value)
  {
    _values[key] = value;
    NotifyStateChanged();
  }

  public string Get(string key)
  {
    return _values[key];
  }

  private void NotifyStateChanged()
  {
    OnChange?.Invoke();
  }
}
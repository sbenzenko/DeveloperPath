const _THEMES = ["ThemeLight", "ThemeDark"];

window.cultureService = {
  get: () => localStorage['DeveloperPathCulture'],
  set: (value) => localStorage['DeveloperPathCulture'] = value
};

window.currentTheme = {
  get: () => {
    let theme = localStorage['DeveloperPathTheme'];
    return _THEMES.includes(theme) ? theme : _THEMES[0]
  },
  set: (value) => {
    localStorage['DeveloperPathTheme'] = value;
    changeTheme();
  }
};

function changeTheme() {
  document.documentElement.className = currentTheme.get();
}

function detectColorScheme() {
  let theme = localStorage['DeveloperPathTheme'];
  if (theme === undefined)
    theme = (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches)
      ? _THEMES[1]
      : _THEMES[0];
  currentTheme.set(theme);
}
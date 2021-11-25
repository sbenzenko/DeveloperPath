window.cultureService = {
  get: () => localStorage['DeveloperPathCulture'],
  set: (value) => localStorage['DeveloperPathCulture'] = value
};
window.currentTheme = {
  get: () => localStorage['DeveloperPathTheme'],
  set: (value) => localStorage['DeveloperPathTheme'] = value
};
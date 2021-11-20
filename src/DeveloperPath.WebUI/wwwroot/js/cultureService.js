window.cultureService = {
    get: () => localStorage['DeveloperPathCulture'],
    set: (value) => localStorage['DeveloperPathCulture'] = value
};
var rootPath=window.location.href;
if (rootPath.indexOf('http://')==0) rootPath = rootPath.substring('http://'.length);
var indexSlash=rootPath.indexOf('/');
rootPath = 'http://' + rootPath.substring(0,indexSlash);
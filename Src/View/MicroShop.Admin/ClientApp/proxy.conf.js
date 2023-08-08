const { env } = require('process');

const target = env.Urls__Admin ? env.Urls__Admin : env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:21901';

console.log('proxy.conf.js', 'target', target);

const PROXY_CONFIG = [
  {
    context: [
      "/config",
    ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    },
    "changeOrigin": true
  }
]

module.exports = PROXY_CONFIG;

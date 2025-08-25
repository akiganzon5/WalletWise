
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/financeapp/',
  locale: undefined,
  routes: undefined,
  entryPointToBrowserMapping: {},
  assets: {
    'index.csr.html': {size: 1181, hash: 'f0bc1736c46f848b37cc26d1a54bf056a81af096a009ec85a0f82e82f45e936f', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1022, hash: 'd166a344bec61e417ff80b8451564ec6e6b41c68c31822452ef10eb41766ae18', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'styles-4S7TYYLD.css': {size: 79847, hash: 'VWvNx/4GkWk', text: () => import('./assets-chunks/styles-4S7TYYLD_css.mjs').then(m => m.default)}
  },
};

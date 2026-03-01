export function initGlobalHttpInterceptor() {
  const originalFetch = window.fetch;

  window.fetch = async (...args) => {
    const response = await originalFetch(...args);

    console.log('%c[uix-library-http]%c fetch : %c','color:green;font-weight:bold', 'color:inherit', args);

    if (response.status === 401) {
      const redirectTo = response.headers.get('X-Redirect-To');
      window.location.href = redirectTo ?? '/sso';
    }

    return response;
  };
}
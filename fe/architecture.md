


nf  komunikacja
1) angular @input, @emit 
   TOOD: opisac
2) Shared Store (NgRx / Zustand) jako singleton shared lib
   TOOD: opisac
3)bus window
wykorzystywane mechanizm przegladrki, uniwersalne 
ts host emit
export function mfEmit(type: string, payload?: any) {
  window.dispatchEvent(new CustomEvent('mf:bus', {
    detail: { type, payload, source: 'portal' }
  }));
}

ts host listen

window.addEventListener('mf:bus', (ev: any) => {
  const { type, payload, source } = ev.detail;
  if (type === 'logout') { ... }
});

remote ts vue  listen
onMounted(() => {
  window.addEventListener('mf:bus', handler);
});

function handler(ev) {
  const { type, payload } = ev.detail;
  if (type === 'context') state.context = payload;
}

remote ts vue emit 
window.dispatchEvent(new CustomEvent('mf:bus', {
  detail: { type: 'logout', payload: null, source: 'menu' }
}));


4) BroadcastChannel
https://developer.mozilla.org/en-US/docs/Web/API/BroadcastChannel


5) https://ngrx.io/guide/store/why
Wspólny storage 



Wiecej 
 BroadcastChannel
https://developer.mozilla.org/en-US/docs/Web/API/BroadcastChannel

Storage 
https://ngrx.io/
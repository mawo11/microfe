const fs = require('fs');
const path = require('path');

const distDir = path.join(__dirname, 'dist', 'uix-library', 'browser');
const outputFile = path.join(distDir, 'elements.js');

if (!fs.existsSync(distDir)) {
  console.error('❌ Brak katalogu dist! Uruchom najpierw: npm run build');
  process.exit(1);
}

console.log('🔍 Szukam plików JS w dist/dist/uix-library/browser...');

// Pobierz wszystkie pliki .js posortowane (polyfills, chunk, main)
const files = fs.readdirSync(distDir)
  .filter(f => f.endsWith('.js') && f !== 'elements.js')
  .sort((a, b) => {
    // polyfills pierwsze, main ostatni
    if (a.startsWith('polyfills')) return -1;
    if (b.startsWith('polyfills')) return 1;
    if (a.startsWith('main')) return 1;
    if (b.startsWith('main')) return -1;
    return a.localeCompare(b);
  });

if (files.length === 0) {
  console.error('❌ Nie znaleziono plików JS w dist/dist/uix-library/browser');
  process.exit(1);
}

let combined = '';
for (const file of files) {
  const filePath = path.join(distDir, file);
  combined += fs.readFileSync(filePath, 'utf8') + '\n';
  console.log(`✓ Dodano: ${file}`);
}

fs.writeFileSync(outputFile, combined);
const sizeKB = (combined.length / 1024).toFixed(1);
console.log(`\n✅ Gotowe! → dist/dist/uix-library/browser/elements.js (${sizeKB} KB)`);
console.log(`\nAby serwować lokalnie:`);
console.log(`  npx http-server dist/uix-library/browser -p 4300 --cors`);

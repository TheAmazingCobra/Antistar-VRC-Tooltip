# Antistar.Tooltip

The Antistar support and FAQ window that comes with our packs. Open it from **Antistar Assets > Support & FAQ**.

Get it from the [Antistar listing](https://theamazingcobra.github.io/Antistar-VCC-Listing/) in VCC. Installing it removes the old copy in `Assets/Antistar Store/Tooltip`.

## Releasing

1. Raise `version` in `Packages/com.antistar.tooltip/package.json`.
2. Run **Build Release** from the Actions tab.

The listing picks the new version up within the hour.

Setup, once: add the repository variable `PACKAGE_NAME` with the value `com.antistar.tooltip` (Settings > Secrets and variables > Actions > Variables).

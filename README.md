# AutoIU
## Co to je 
AutoIU (Automatická instalace učebny) je program pro bezobslužnou instalaci operačního systému Windows a programů na velké množství počítačů.
## Jak to funguje
Tento program obsahuje a generuje skripty pro bezobslužnou instalaci přes PXE za použití TinyPXE, iPXE a Windows PE. Na počítači se spustí Windows PE který připojí SMB share ze serveru který obsahuje instalaci Windows, tu spustí s prametry z autounattend.xml
## Jak se používá
1. Spustíte program
2. Zvolíte možnosti instalace
3. Stisknete tlačítko Spustit
4. Otevře se vám program TinyPXE, v něm stačí pouze stisknout tlačítko Online
5. Pokud program najde DHCP server na síti tak nezapne svůj DHCP server a musíte nastavit vaše DHCP tak že PXE server je počítač ze kterého instalujete
6. Nastavíme počítač tak aby startoval z PXE
7. Nyní na všech počítačích se spustí nabídka ve které vyberete možnost instalovat
8. Nyní je bezobslužná instalace spuštěna a stačí počkat než se vše nainstaluje9
9. nashle
10. 

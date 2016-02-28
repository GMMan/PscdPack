using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PscdPack
{
    /// <summary>
    /// Modes for on-cartridge memory.
    /// </summary>
    public enum ExtraSaveMode
    {
        /// <summary>
        /// No on-cartridge memory.
        /// </summary>
        None,
        /// <summary>
        /// Cartridge contains SRAM.
        /// </summary>
        Sram,
        /// <summary>
        /// Cartridge contains EEPROM operating in mode 1.
        /// </summary>
        EepromMode1,
        /// <summary>
        /// Cartridge contains EEPROM operating in mode 2.
        /// </summary>
        EepromMode2,
        /// <summary>
        /// Cartridge uses the Acclaim 32M mapper.
        /// </summary>
        EepromAcclaim32M,
        /// <summary>
        /// Disable emulation of SEGA 315-5779 mapper bank switching mechanism.
        /// </summary>
        DisableBankSwitching
    }
}

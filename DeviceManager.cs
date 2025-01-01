// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Original Code by TheJebForge (https://github.com/TheJebForge)

using HidApi;

namespace ElgatoStreamDeck;

/**
 * Instantiates HID library under the hood, only one instance should exist
 */

/// <summary>
/// Provides an instance of the DeviceManager to scan for, and connect Elgato Stream Deck Devices.
/// This class is a Singleton Pattern, use <see cref="DeviceManager.Get"/> to fetch the instnace.
/// </summary>
public class DeviceManager {
    private const ushort ElgatoVendorId = 0x0fd9;

    private static DeviceManager? _instance;

    private readonly SafeHidHandle _hid;

    private DeviceManager() => _hid = new SafeHidHandle();

    /// <summary>
    /// Enumerates through the list of Connected Devices, returning (<see cref="Kind"/>, serialNumber) to allow
    /// connecting to the device.  To connect to the device, see either: <see cref="DeviceManager.ConnectDevice"/>
    /// or <see cref="DeviceManager.ConnectDeviceConcurrent"/>
    /// </summary>
    /// <returns>IEnumerable&lt;(Kind, string)&gt;</returns>
    public System.Collections.Generic.IEnumerable<(Kind, string)> ListDevices() {
        return Hid.Enumerate(ElgatoVendorId)
            .Select(info => (info.ProductId.ToKind(), info.SerialNumber));
    }

    /// <summary>
    /// Creates a new <see cref="Device"/> connection to allow processing of events, and storing of images.
    /// For use with Thread Safe, <seealso cref="DeviceManager.ConnectDeviecConcurrent"/>.
    /// </summary>
    /// <param name="kind">The <see cref="Kind"/> of device.</param>
    /// <param name="serial">The string serial number of device</param>
    /// <returns><see cref="Device"/> handle, for interacting with the specified device.</returns>
    /// <exception cref="ArgumentException">Thrown when it cannot recognize the device.</exception>
    public Device ConnectDevice(Kind kind, string serial) {
        if (kind == Kind.Unknown) throw new ArgumentException("Can't connect to unrecognized device kind");

        return new Device(new HidApi.Device(ElgatoVendorId, kind.ToPid(), serial), kind);
    }

    /// <summary>
    /// Creates a new <see cref="ConcurrentDevice"/> thread safe connection to allow processing of events and storing of images.
    /// For use in Single Thread mode, <seealso cref="DeviceManager.ConnectDevice"/>.
    /// </summary>
    /// <param name="kind">The <see cref="Kind"/> of device.</param>
    /// <param name="serial">The string serial number of device</param>
    /// <returns><see cref="ConcurrentDevice"/> handle, for interacting with the specified device.</returns>
    /// <exception cref="ArgumentException">Thrown when it cannot recognize the device.</exception>
    public ConcurrentDevice ConnectDeviceConcurrent(Kind kind, string serial) {
        if (kind == Kind.Unknown) throw new ArgumentException("Can't connect to unrecognized device kind");

        return new ConcurrentDevice(new Device(new HidApi.Device(ElgatoVendorId, kind.ToPid(), serial), kind));
    }

    /**
     * Should be called when you'll never use the manager again, frees the HID library
     */
    
    /// <summary>
    /// Unloads DeviceManager when no longer to be used.
    /// </summary>
    public void Dispose() {
        _hid.Dispose();
        _instance = null;
    }

    /// <summary>
    /// Fetches the singleton Instance of the Device Manager.
    /// </summary>
    /// <returns><see cref="DeviceManager"/> instance.</returns>
    public static DeviceManager Get() {
        _instance ??= new DeviceManager();
        return _instance;
    }
}
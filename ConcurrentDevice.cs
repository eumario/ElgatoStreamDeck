// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Original Code by TheJebForge (https://github.com/TheJebForge)

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ElgatoStreamDeck;

/// <summary>
/// A Thread Safe wrapper around Elgato Stream Deck device.
/// </summary>
/// <param name="device">Stream Deck Device opened from <see cref="DeviceManager.ConnectDevice"/>, is also return from <seealso cref="DeviceManager.ConnectDeviceConcurrent"/>.</param>
public class ConcurrentDevice(Device device) : IDevice {
	private readonly Kind _kind = device.Kind();

	/// <summary>
	/// Return the <see cref="Kind"/> of Stream Deck Device.
	/// </summary>
	public Kind Kind() => _kind;

	/// <summary>
	/// Returns the Manufacturer for the device as a string.
	/// </summary>
	public string Manufacturer() {
		lock (device) {
			return device.Manufacturer();
		}
	}

	/// <summary>
	/// Returns the Product Name for said device as a string.
	/// </summary>
	public string Product() {
		lock (device) {
			return device.Product();
		}
	}

	/// <summary>
	/// Returns the Serial Number for said device as a string.
	/// </summary>
	public string SerialNumber() {
		lock (device) {
			return device.SerialNumber();
		}
	}

	/// <summary>
	/// Returns the Firmware Version for the device as a string.
	/// </summary>
	public string FirmwareVersion() {
		lock (device) {
			return device.FirmwareVersion();
		}
	}

	/// <summary>
	/// Returns the Input events of the Stream Deck device in this polling of it.
	/// </summary>
	/// <param name="timeout">Time to wait before failing the reading of the input.</param>
	/// <returns>An instance of <see cref="Input"/> representing the event if event occurs, otherwise null if nothing was read in the timeout period.</returns>
	public Input? ReadInput(int? timeout) {
		lock (device) {
			return device.ReadInput(timeout);
		}
	}

	/// <summary>
	/// Resets the Stream Deck Device to First Power On state. (Default brightness, Default Boot Logo for buttons.)
	/// </summary>
	public void Reset() {
		lock (device) {
			device.Reset();
		}
	}

	/// <summary>
	/// Sets the Stream Deck Device brightness to the specified percentage.
	/// </summary>
	/// <param name="percent">Percentage of backlighting to the Stream Deck Device.</param>
	public void SetBrightness(byte percent) {
		lock (device) {
			device.SetBrightness(percent);
		}
	}

	/// <summary>
	/// Draws an Image starting from the specified Key, till it runs out of buttons to draw it to.
	/// </summary>
	/// <param name="keyIndex">The key Index to start at.</param>
	/// <param name="imageData">The raw JPG/BMP Image data to be stored for Key.</param>
	public void WriteImage(byte keyIndex, ReadOnlySpan<byte> imageData) {
		lock (device) {
			device.WriteImage(keyIndex, imageData);
		}
	}

	/// <summary>
	/// Draws Image data to the Lcd Strip on a Stream Deck +
	/// </summary>
	/// <param name="x">X Position to start at</param>
	/// <param name="y">Y Position to start at</param>
	/// <param name="w">Width to draw</param>
	/// <param name="h">Height to draw</param>
	/// <param name="imageData">The raw JPG/BMP Image data to be drawn to the Lcd Screen.</param>
	public void WriteLcd(ushort x, ushort y, ushort w, ushort h, ReadOnlySpan<byte> imageData) {
		lock (device) {
			device.WriteLcd(x, y, w, h, imageData);
		}
	}

	/// <summary>
	/// Clears the Image set for a specific Key.
	/// </summary>
	/// <param name="keyIndex">Index of key to be cleared.</param>
	public void ClearButtonImage(byte keyIndex) {
		lock (device) {
			device.ClearButtonImage(keyIndex);
		}
	}

	/// <summary>
	/// Sets the Image for a specific Key.
	/// </summary>
	/// <param name="keyIndex">Index of key to be set.</param>
	/// <param name="image">A <see cref="SixLabors.ImageSharp.Image">Image</see> to set the button to.</param>
	public void SetButtonImage(byte keyIndex, Image image) {
		lock (device) {
			device.SetButtonImage(keyIndex, image);
		}
	}

	/// <summary>
	/// Sets the Image for a specific Key.
	/// </summary>
	/// <param name="keyIndex">Index of key to be set.</param>
	/// <param name="image">A <see cref="SixLabors.ImgaeSharp.Image&lt;Rgb24&gt;">Image&lt;Rgb24&gt;</see> to set the button to.</param>
	public void SetButtonImage(byte keyIndex, Image<Rgb24> image) {
		lock (device) {
			device.SetButtonImage(keyIndex, image);
		}
	}

	/// <summary>
	/// Sets the Image for a specific Key.
	/// </summary>
	/// <param name="keyIndex">Index of key to be set.</param>
	/// <param name="image">A ReadOnlySpan&lt;byte&gt; raw data to be drawn to the screen</param>
	/// <param name="width">Width of the Image</param>
	/// <param name="height">Height of the Image</param>
	public void SetButtonImage(byte keyIndex, ReadOnlySpan<byte> image, int width, int height) {
		lock (device) {
			device.SetButtonImage(keyIndex, image, width, height);
		}
	}

	/// <summary>
	/// Free's up the Device, when no longer in use.
	/// </summary>
	public void Dispose() {
		device.Dispose();
		GC.SuppressFinalize(this);
	}
}
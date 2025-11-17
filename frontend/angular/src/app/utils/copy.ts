export async function copyToClipboard(text: string): Promise<boolean> {
  try {
    const value = (text ?? '').toString();
    if (!value.trim().length) {
      console.log('No text to copy');
      return false;
    }

    if (!navigator.clipboard?.writeText) {
      console.log('Clipboard API not available');
      return false;
    }

    await navigator.clipboard.writeText(value);
    return true;
  } catch (error) {
    console.error('Failed to copy text to clipboard:', error);
    return false;
  }
}

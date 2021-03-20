import {expect, device, element, by} from 'detox';

describe('Home Screen', () => {
  beforeAll(async () => {
    await device.launchApp();
  });

  beforeEach(async () => {
    await device.reloadReactNative();
  });

  it('should have ScrollView', async () => {
    await expect(element(by.id('home.scrollView'))).toExist();
  });
});

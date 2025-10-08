import axios from 'axios';

export type DeviceDto = {
  id: number;
  name: string;
  type: string;
  description: string;
  isAvailable: boolean;
  currentSessionId?: number | null;
};

export const getDevices = async (): Promise<DeviceDto[]> => {
  const res = await axios.get<DeviceDto[]>('/api/devices');
  return res.data;
};

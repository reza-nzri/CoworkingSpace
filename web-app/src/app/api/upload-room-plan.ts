import { promises as fs } from 'fs';
import path from 'path';
import { NextApiRequest, NextApiResponse } from 'next';

export const config = {
  api: {
    bodyParser: {
      sizeLimit: '5mb',  // Limit image size
    },
  },
};

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  if (req.method === 'POST') {
    try {
      const file = req.body;
      const fileName = req.query.fileName as string;

      const uploadDir = path.join(process.cwd(), 'public/company/room/map-cloud');
      await fs.mkdir(uploadDir, { recursive: true });

      const filePath = path.join(uploadDir, fileName);
      await fs.writeFile(filePath, Buffer.from(file), 'binary');

      return res.status(200).json({ message: 'Upload successful' });
    } catch (error) {
      console.error('Error uploading file:', error);
      return res.status(500).json({ message: 'Failed to upload file' });
    }
  } else {
    res.status(405).json({ message: 'Method Not Allowed' });
  }
}

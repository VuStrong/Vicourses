import type { Config } from 'tailwindcss'
import withMT from "@material-tailwind/react/utils/withMT";

const config: Config = {
  content: [
    './src/pages/**/*.{js,ts,jsx,tsx,mdx}',
    './src/components/**/*.{js,ts,jsx,tsx,mdx}',
    './src/app/**/*.{js,ts,jsx,tsx,mdx}',
  ],
  theme: {
    extend: {
      colors: {
        'primary': '#007140',
        'success': '#1ed760',
        'error': '#f44563',
      }
    },
  },
  plugins: []
}
export default withMT(config)
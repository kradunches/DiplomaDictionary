import http from 'k6/http';
import {check, sleep} from 'k6';
import {htmlReport} from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';

// средняя нагрузка
export const options = {
    scenarios: {
        average_load: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                {duration: '2m', target: 10},  // разогрев до 10 VUs
                {duration: '3m', target: 20},  // рост до средней нагрузки
                {duration: '10m', target: 20}, // стабильная средняя нагрузка
                {duration: '2m', target: 0},   // плавное снижение
            ],
            gracefulRampDown: '30s',
        },
    },
    thresholds: {
        http_req_failed: ['rate<0.01'], // ошибок < 1%
        http_req_duration: [
            'avg<500',   // средняя латентность
            'p(95)<800', // p95
            'p(99)<1200' // p99
        ],
    },
};

export default function () {
    const res = http.get('http://localhost:5093/controller/GetAllTerms');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1); // секунды между запросами одного VU
}

export function handleSummary(data) {
    return {
        'results/average-load_getallterms_report.html': htmlReport(data),
    };
}